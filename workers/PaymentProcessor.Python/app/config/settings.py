import os
from azure.identity import DefaultAzureCredential
from azure.keyvault.secrets import SecretClient


def require_env(name: str) -> str:
    value = os.getenv(name)
    if not value:
        raise RuntimeError(f"Missing required environment variable: {name}")
    return value


def load_settings():
    use_key_vault = os.getenv("USE_KEY_VAULT", "false").lower() == "true"
    use_sasl = os.getenv("Kafka__UseSasl", "false").lower() == "true"

    # -------- LOCAL / DOCKER / CI --------
    if not use_key_vault:
        print("Loading Kafka settings from environment variables")

        settings = {
            "bootstrap_servers": require_env("Kafka__BootstrapServers"),
            "topic": require_env("Kafka__PaymentTopic"),
            "topic_retry1": require_env("Kafka__Retry1Topic"),
            "topic_retry2": require_env("Kafka__Retry2Topic"),
            "topic_deadletter": require_env("Kafka__PaymentDead"),
            "topic_invalidPayments": require_env("Kafka__PaymentInvalid"),

            # ALWAYS PRESENT
            "use_sasl": use_sasl,
            "sasl_username": None,
            "sasl_password": None,
        }

        if use_sasl:
            settings["sasl_username"] = require_env("Kafka__SaslUsername")
            settings["sasl_password"] = require_env("Kafka__SaslPassword")

        return settings

    # -------- AZURE / PROD --------
    print("Loading Kafka settings from Azure Key Vault")

    key_vault_url = require_env("KeyVault__Url")

    credential = DefaultAzureCredential()
    client = SecretClient(vault_url=key_vault_url, credential=credential)

    return {
        "bootstrap_servers": client.get_secret("Kafka-BootstrapServers").value,
        "topic": client.get_secret("Kafka-PaymentTopic").value,
        "topic_retry1": client.get_secret("Kafka-Retry1Topic").value,
        "topic_retry2": client.get_secret("Kafka-Retry2Topic").value,
        "topic_deadletter": client.get_secret("Kafka-PaymentDead").value,
        "topic_invalidPayments": client.get_secret("Kafka-PaymentInvalid").value,

        "use_sasl": True,
        "sasl_username": client.get_secret("Kafka-SaslUsername").value,
        "sasl_password": client.get_secret("Kafka-SaslPassword").value,
    }
