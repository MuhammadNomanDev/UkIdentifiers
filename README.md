# UkIdentifiers

Validation and parsing for UK-specific identifiers: National Insurance numbers, postcodes, NHS numbers, and more.

Built with Muse AI pair-programming (Muse Spark by Meta) — all design decisions reviewed and understood by the author.

## Usage

```csharp
using UkIdentifiers;

NationalInsuranceNumber.IsValid("AB123456C");  // true
UkPostcode.Normalize("sw1a1aa");               // "SW1A 1AA"
UkPostcode.Split("M1 1AE");                    // ("M1", "1AE")
NhsNumber.IsValid("4505577104");               // true (Mod-11 check digit)
UkVatNumber.IsValid("GB123456789");            // true
CompaniesHouseNumber.IsValid("AB123456");      // true
```

## Identifiers

| Type | Validator | Notes |
|------|-----------|-------|
| National Insurance | `NationalInsuranceNumber` | Format + HMRC prefix rules |
| Postcode | `UkPostcode` | BS 7666 patterns, normalize, split |
| NHS number | `NhsNumber` | 10 digits, Mod-11 check digit |
| UTR | `UniqueTaxpayerReference` | 10 digits, format only (HMRC algorithm not public) |
| VAT number | `UkVatNumber` | GB/XI prefix + 9 digits (12 for branch traders), format only |
| Company number | `CompaniesHouseNumber` | 8 digits or 2 letters + 6 digits, format only |

## Validation vs verification

Format validation answers "could this be real?" — checksums catch typos but
can't confirm issuance. Verification ("is this real?") needs an HMRC /
Companies House lookup and is out of scope.
