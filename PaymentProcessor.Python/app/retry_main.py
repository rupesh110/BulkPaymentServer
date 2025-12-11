from config import load_settings
from kafka_client import create_retry_consumer
from kafka_client import create_producer
from services import process_payment

def run_retry_worker():
	print("Loading Kafka settings from Azure Key Vault...")
	settings = load_settings()

	print("Settings loaded:")
	print(f"  → Bootstrap: {settings['bootstrap_servers']}")
	print(f"  → Retry Topic:     {settings['topic_retry2']}")

	consumer = create_retry_consumer(settings)
	producer = create_producer(settings)
	print("Retry Consumer connected. Listening for messages...")

	retry_topic_2 = settings["topic_retry2"]

	for msg in consumer:
		event = msg.value
		process_payment(event, producer, retry_topic_2)
		consumer.commit()


if __name__ == "__main__":
	run_retry_worker()

