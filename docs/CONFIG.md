# Configuration Reference

## Kafka

Provided by:
- Infrastructure (Kafka cluster – Confluent)

Consumed at runtime by:
- BulkPayment.Api
- PaymentProcessor.Python

Accessed via (code layer):
- BulkPayment.Infrastructure (Kafka client implementations)
- Python Kafka client (for PaymentProcessor.Python)

Variables:
- KAFKA_BOOTSTRAP_SERVERS
- KAFKA_PAYMENT_TOPIC
- KAFKA_RETRY_TOPIC
- KAFKA_DLQ_TOPIC

Purpose:
Allows services to publish and consume payment events.

---

## Blob Storage (Azure)

Provided by:
- Infrastructure (Azure Blob Storage)

Consumed at runtime by:
- BulkPayment.Api

Accessed via (code layer):
- BulkPayment.Infrastructure (Blob storage client)

Variables:
- BLOB_CONNECTION_STRING
- BLOB_CONTAINER_NAME

Purpose:
CSV upload storage. Workers do not access blobs directly.

---

## Database

Provided by:
- Infrastructure (Azure SQL / SQL Server)

Consumed at runtime by:
- BulkPayment.Api
- PaymentProcessor.Python

Accessed via (code layer):
- BulkPayment.Infrastructure (EF Core repositories)

Variables:
- DB_CONNECTION_STRING

Purpose:
Persistent storage for upload metadata and processing state.
