#!/usr/bin/env python3
"""Validate RFQ eval fixtures against the Phase 0 schema.

Phase 1 scope: there is no extraction engine yet, so this harness only
validates STRUCTURE — that every fixture in the manifest has an input file
and an expert-reviewed expected ground truth whose field keys exist in the
schema. Once an extraction engine exists (Phase 4+), a scoring mode will be
added to compare engine output to these expected files.

Usage:
    python evals/scripts/validate_fixtures.py

Exit code 0 = all fixtures structurally valid, non-zero = problems found.
"""
from __future__ import annotations

import json
import sys
from pathlib import Path

try:
    import yaml
except ImportError:
    print("ERROR: pyyaml not installed. Run: pip install pyyaml", file=sys.stderr)
    sys.exit(2)

REPO_ROOT = Path(__file__).resolve().parents[2]
EVALS_DIR = REPO_ROOT / "evals"
SCHEMA_YAML = REPO_ROOT / "docs" / "domain" / "pressure-dp-required-fields-v0.yaml"
MANIFEST = EVALS_DIR / "manifest.json"

VALID_STATUSES = {
    "provided", "extracted", "missing", "unknown",
    "unverified", "approved", "rejected", "conflicting",
}


def load_schema_field_keys() -> set[str]:
    data = yaml.safe_load(SCHEMA_YAML.read_text(encoding="utf-8"))
    keys = {f["key"] for f in data.get("fields", [])}
    keys |= {f["key"] for f in data.get("commercial_fields", [])}
    return keys


def main() -> int:
    problems: list[str] = []

    if not MANIFEST.exists():
        print(f"ERROR: manifest not found at {MANIFEST}", file=sys.stderr)
        return 2

    schema_keys = load_schema_field_keys()
    manifest = json.loads(MANIFEST.read_text(encoding="utf-8"))
    fixtures = manifest.get("fixtures", [])

    if not fixtures:
        print("WARNING: no fixtures declared in manifest yet.")
        return 0

    for fx in fixtures:
        fid = fx.get("id", "<no-id>")
        input_path = EVALS_DIR / fx.get("input", "")
        expected_path = EVALS_DIR / fx.get("expected", "")

        if not input_path.exists():
            problems.append(f"[{fid}] input file missing: {input_path}")
        if not expected_path.exists():
            problems.append(f"[{fid}] expected file missing: {expected_path}")
            continue

        try:
            expected = json.loads(expected_path.read_text(encoding="utf-8"))
        except json.JSONDecodeError as exc:
            problems.append(f"[{fid}] expected JSON invalid: {exc}")
            continue

        for req in expected.get("requirements", []):
            key = req.get("field_key")
            status = req.get("status")
            if key not in schema_keys:
                problems.append(f"[{fid}] unknown field_key not in schema: {key}")
            if status not in VALID_STATUSES:
                problems.append(f"[{fid}] invalid status '{status}' for field '{key}'")
            # Missing must have null value; verified-ish must not be null.
            if status == "missing" and req.get("normalized_value") is not None:
                problems.append(f"[{fid}] field '{key}' status=missing but value is not null")

        for mf in expected.get("expected_missing_fields", []):
            key = mf.get("field_key")
            if key not in schema_keys:
                problems.append(f"[{fid}] expected_missing_fields references unknown key: {key}")

    if problems:
        print("FIXTURE VALIDATION FAILED:")
        for p in problems:
            print(f"  - {p}")
        return 1

    print(f"OK: {len(fixtures)} fixture(s) structurally valid against schema "
          f"({len(schema_keys)} known field keys).")
    return 0


if __name__ == "__main__":
    sys.exit(main())
