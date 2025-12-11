from kafka import KafkaProducer
import json

def create_producer(settings):
	return KafkaProducer(
		bootstrap_servers=settings["bootstrap_servers"],
		security_protocol="SASL_SSL",
		sasl_mechanism="PLAIN",
		sasl_plain_username=settings["sasl_username"],
		sasl_plain_password=settings["sasl_password"],
		value_serializer=lambda v: json.dumps(v).encode("utf-8"),
	)