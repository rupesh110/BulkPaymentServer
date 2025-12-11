import os
from azure.identity import DefaultAzureCredential
from azure.keyvault.secrets import SecretClient

def load_settings():
    key_vault_url = os.getenv("BULKPAYMENT_KV_URL")

    if not key_vault_url:
        raise ValueError("BULKPAYMENT_KV_URL environment variable is not set.")

    credential = DefaultAzureCredential()
    secret_client = SecretClient(vault_url=key_vault_url, credential=credential)

    settings = {
        "bootstrap_servers": secret_client.get_secret("Kafka-BootstrapServers").value,
        "sasl_username": secret_client.get_secret("Kafka-SaslUsername").value,
        "sasl_password": secret_client.get_secret("Kafka-SaslPassword").value,

        #MAIN topic
        "topic": secret_client.get_secret("Kafka-TestTopic").value,

        #Retry topics
        "topic_retry1": secret_client.get_secret("Kafka-Retry1Topic").value,
        "topic_retry2": secret_client.get_secret("Kafka-Retry2Topic").value,
    }

    return settings
