from config import load_settings
from kafka_client import create_consumer

def run_worker():
    print("Loading Kafka settings from Azure Key Vault...")

    settings = load_settings()

    print("Settings loaded:")
    print(f"  → Bootstrap: {settings['bootstrap_servers']}")
    print(f"  → Topic:     {settings['topic']}")

    consumer = create_consumer(settings)

    print("Consumer connected. Listening for messages...")

    for msg in consumer:
        print("📥 Received:", msg.value)


if __name__ == "__main__":
    run_worker()
