from kafka import KafkaConsumer
from utils import safe_json

def create_retry_consumer(settings):
    topic = settings["topic_retry1"]

    common_args = dict(
        bootstrap_servers=settings["bootstrap_servers"],
        auto_offset_reset="earliest",
        enable_auto_commit=True,
        value_deserializer=lambda m: safe_json(m),
        group_id="bulkpayment-worker-retry1",
    )

    if settings["use_sasl"]:
        consumer = KafkaConsumer(
            topic,
            security_protocol="SASL_SSL",
            sasl_mechanism="PLAIN",
            sasl_plain_username=settings["sasl_username"],
            sasl_plain_password=settings["sasl_password"],
            **common_args
        )
    else:
        consumer = KafkaConsumer(
            topic,
            **common_args
        )

    print(
        f"Retry consumer connected | "
        f"bootstrap={settings['bootstrap_servers']} | "
        f"sasl={settings['use_sasl']} | "
        f"topic={topic}"
    )

    return consumer
