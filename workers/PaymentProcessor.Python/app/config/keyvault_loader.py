import os
from azure.identity import DefaultAzureCredential
from azure.keyvault.secrets import SecretClient


KEY_VAULT_URL = "https://bulkpaymentsecret.vault.azure.net"


def load_settings():
    # Allow override (local / test)
    key_vault_url = os.getenv("BULKPAYMENT_KV_URL", KEY_VAULT_URL)

    # LOCAL / DOCKER / CI MODE
    if os.getenv("USE_KEY_VAULT", "false").lower() != "true":
        print("🔹 Loading settings from environment variables")

        return {
            "bootstrap_servers": os.environ["Kafka__BootstrapServers"],
            "sasl_username": os.environ["Kafka__SaslUsername"],
            "sasl_password": os.environ["Kafka__SaslPassword"],

            "topic": os.environ["Kafka__PaymentTopic"],
            "topic_retry1": os.environ["Kafka__Retry1Topic"],
            "topic_retry2": os.environ["Kafka__Retry2Topic"],
            "topic_deadletter": os.environ["Kafka__PaymentDead"],
            "topic_invalidPayments": os.environ["Kafka__PaymentInvalid"],
        }

    # AZURE / PROD MODE
    print("🔐 Loading Kafka settings from Azure Key Vault")

    credential = DefaultAzureCredential()
    client = SecretClient(vault_url=key_vault_url, credential=credential)

    return {
        "bootstrap_servers": client.get_secret("Kafka-BootstrapServers").value,
        "sasl_username": client.get_secret("Kafka-SaslUsername").value,
        "sasl_password": client.get_secret("Kafka-SaslPassword").value,

        "topic": client.get_secret("Kafka-PaymentTopic").value,
        "topic_retry1": client.get_secret("Kafka-Retry1Topic").value,
        "topic_retry2": client.get_secret("Kafka-Retry2Topic").value,
        "topic_deadletter": client.get_secret("Kafka-PaymentDead").value,
        "topic_invalidPayments": client.get_secret("Kafka-PaymentInvalid").value,
    }
