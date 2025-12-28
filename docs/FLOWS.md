## Payment Upload & Processing Flow

1. Client uploads CSV via `POST /uploads`
2. API validates file structure
3. API stores CSV in Blob Storage
4. API publishes `PaymentRequested` event
5. Worker consumes event
6. Worker retries up to 3 times
7. Permanent failures go to DLQ

### Failure Handling
- Validation failure → HTTP 400
- Kafka unavailable → HTTP 503
- Worker crash → message reprocessed

### Notes
- Kafka is the source of truth
- Blob storage is write-once
