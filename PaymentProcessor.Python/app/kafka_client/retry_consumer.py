from kafka import KafkaConsumer
from utils import safe_json


def create_retry_consumer(settings):
	topic = settings["topic_retry1"]

	consumer = KafkaConsumer(
		topic,
		bootstrap_servers=settings["bootstrap_servers"],
		security_protocol="SASL_SSL",
		sasl_mechanism="PLAIN",
		sasl_plain_username=settings["sasl_username"],
		sasl_plain_password=settings["sasl_password"],
		auto_offset_reset="earliest",
		enable_auto_commit=True,
		value_deserializer=lambda m: safe_json(m),
		group_id="bulkpayment-worker-retry1"
	)

	print(f"Retry Consumer connected to: {topic}")
	return consumer