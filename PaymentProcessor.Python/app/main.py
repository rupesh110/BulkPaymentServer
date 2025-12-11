from config import load_settings
from kafka_client import create_consumer
from services import process_payment
from kafka_client import create_producer

def run_worker():
    print("Loading Kafka settings from Azure Key Vault...")

    settings = load_settings()

    print("Settings loaded:")
    print(f"  → Bootstrap: {settings['bootstrap_servers']}")
    print(f"  → Topic:     {settings['topic']}")

    consumer = create_consumer(settings)
    producer = create_producer(settings)

    retry_topic = settings["topic_retry1"]

    print("Consumer connected. Listening for messages...")

    for msg in consumer:
        event = msg.value
        process_payment(event, producer, retry_topic)
        consumer.commit()  



if __name__ == "__main__":
    run_worker()
