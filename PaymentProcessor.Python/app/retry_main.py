from config import load_settings
from kafka_client import create_retry_consumer
from kafka_client import create_producer
from services import retry_payment

def run_retry_worker():
	print("Loading Kafka settings from Azure Key Vault...")
	settings = load_settings()

	consumer = create_retry_consumer(settings)
	producer = create_producer(settings)
	print("Retry Consumer connected. Listening for messages...")

	retry_topic = settings["topic_retry1"]
	dead_letter_topic = settings["topic_deadletter"]

	for msg in consumer:
		event = msg.value
		retry_payment(event, producer, retry_topic, dead_letter_topic)
		consumer.commit()


if __name__ == "__main__":
	run_retry_worker()
