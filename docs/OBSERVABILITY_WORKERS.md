# Payment Processor Worker – Observability

This document defines **what the Payment Processor Worker logs and how to debug it**.

---

## 1. Scope

This document applies **only to the Payment Processor Worker**.

The worker:
- Consumes payment events from Kafka
- Processes payments
- Handles retries
- Sends messages to retry topics or DLQ

---

## 2. Correlation ID

### Primary Correlation ID
- **paymentId**

### Rules
- `paymentId` is used as the **Kafka message key**
- `paymentId` must appear in **all worker logs**
- `paymentId` must be preserved across:
  - retries
  - retry topics
  - DLQ

---

## 3. Logging Principles

- Logging is configured once at worker startup
- Each file uses its own logger (`logging.getLogger(__name__)`)
- Logs include business context
- Logs must not include PII (bank details, card numbers)
- Logs are written to stdout

---

## 4. Logged Events

### Payment Lifecycle

| Event | Level | Description |
|-----|------|------------|
| payment_received | INFO | Worker received a payment message |
| payment_processing | INFO | Payment processing started |
| payment_approved | INFO | Payment processed successfully |
| payment_rejected | WARNING | Business rule rejection |
| payment_retry | WARNING | Payment sent to retry topic |
| payment_dlq | ERROR | Payment sent to DLQ |
| infra_error | ERROR | Unexpected worker error |

Each log event includes:
- `paymentId`
- `retryCount`

---

## 5. Error Classification

### Business Errors
Examples:
- Invalid payload
- Amount exceeds limit

Handling:
- Logged as `WARNING`
- Message retried or rejected
- Worker continues processing

---

### Infrastructure Errors
Examples:
- Kafka send or consume failure
- Serialization errors
- Unexpected exceptions

Handling:
- Logged as `ERROR`
- Stack trace logged
- Worker restarts

---

## 6. Retry & DLQ Observability

Retry metadata is stored in the message:

- `RetryCount`
- `LastFailureReason`

Rules:
- RetryCount increments on each retry
- Message is retried until max retries are reached
- After max retries, message is sent to DLQ
- DLQ events are logged as `ERROR`

---

## 7. How to Debug a Payment

1. Obtain the `paymentId`
2. Search worker logs for that `paymentId`
3. Follow the sequence:
   - payment_received
   - payment_processing
   - payment_retry (if any)
   - payment_approved OR payment_dlq
4. Check retry count and failure reason

---

## 8. Non-Goals

This document does **not** cover:
- API observability
- Infrastructure logging
- Kafka broker observability

---

## 9. Summary

- Observability is owned by the Payment Processor Worker
- Logs are designed for payment-level debugging
- `paymentId` is the single source of truth
