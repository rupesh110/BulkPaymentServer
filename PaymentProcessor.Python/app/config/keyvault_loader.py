import os
from azure.identity import DefaultAzureCredential
from azure.keyvault.secrets import SecretClient


def load_settings():
    key_vault_url = os.getenv("BULKPAYMENT_KV_URL")

    # LOCAL / DOCKER / CI MODE
    if not key_vault_url:
        print("Key Vault not configured. Loading settings from environment variables...")

        return {
            "bootstrap_servers": os.environ["Kafka__BootstrapServers"],
            "sasl_username": os.environ["Kafka__SaslUsername"],
            "sasl_password": os.environ["Kafka__SaslPassword"],

            # Main topic
            "topic": os.environ["Kafka__TestTopic"],

            # Retry topics
            "topic_retry1": os.environ["Kafka__Retry1Topic"],
            "topic_retry2": os.environ["Kafka__Retry2Topic"],
        }

    # AZURE / PROD MODE
    print("Loading Kafka settings from Azure Key Vault...")

    credential = DefaultAzureCredential()
    secret_client = SecretClient(
        vault_url=key_vault_url,
        credential=credential
    )

    return {
        "bootstrap_servers": secret_client.get_secret("Kafka-BootstrapServers").value,
        "sasl_username": secret_client.get_secret("Kafka-SaslUsername").value,
        "sasl_password": secret_client.get_secret("Kafka-SaslPassword").value,

        # Main topic
        "topic": secret_client.get_secret("Kafka-TestTopic").value,

        # Retry topics
        "topic_retry1": secret_client.get_secret("Kafka-Retry1Topic").value,
        "topic_retry2": secret_client.get_secret("Kafka-Retry2Topic").value,
    }
