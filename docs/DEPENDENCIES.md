## Dependencies

### BulkPayment.Api
- Kafka
  - Topic: payments
  - Purpose: emits payment instructions
- Azure Blob Storage
  - Purpose: store uploaded CSV files
- SQL Database
  - Purpose: store upload metadata

### PaymentProcessor.Worker
- Kafka
  - Topic: payments (consume)
  - Topic: payments-retry (consume)
  - Topic: payments-dlq(produce)

- SQL Database #TODO
	- Purpose: follows acid principle as in financial services 
