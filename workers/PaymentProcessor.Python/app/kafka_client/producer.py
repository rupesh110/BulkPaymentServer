from kafka import KafkaProducer
from utils import safe_json

def create_producer(settings):
    common_args = dict(
        bootstrap_servers=settings["bootstrap_servers"],
        value_serializer=lambda v: safe_json(v, to_bytes=True),
        retries=5,
        linger_ms=10,
    )

    if settings["use_sasl"]:
        producer = KafkaProducer(
            security_protocol="SASL_SSL",
            sasl_mechanism="PLAIN",
            sasl_plain_username=settings["sasl_username"],
            sasl_plain_password=settings["sasl_password"],
            **common_args
        )
    else:
        producer = KafkaProducer(**common_args)

    print(
        f"Kafka producer connected | "
        f"bootstrap={settings['bootstrap_servers']} | "
        f"sasl={settings['use_sasl']}"
    )

    return producer
