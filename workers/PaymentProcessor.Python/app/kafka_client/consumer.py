from kafka import KafkaConsumer
from utils import safe_json

def create_consumer(settings):
    bootstrap_servers = settings["bootstrap_servers"]
    api_key = settings["sasl_username"]
    api_secret = settings["sasl_password"]
    topic = settings["topic"]

    consumer = KafkaConsumer(
        topic,
        bootstrap_servers=bootstrap_servers,
        security_protocol="SASL_SSL",
        sasl_mechanism="PLAIN",
        sasl_plain_username=api_key,
        sasl_plain_password=api_secret,
        auto_offset_reset="earliest",
        enable_auto_commit=True,
        value_deserializer=lambda m: safe_json(m),
        group_id="bulkpayment-worker-main",
        auto_commit_interval_ms=5000,
    )

    print(f"Kafka consumer connected to topic '{topic}'")

    return consumer
