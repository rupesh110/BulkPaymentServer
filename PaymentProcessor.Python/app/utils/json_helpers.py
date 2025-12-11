import json

import json

def safe_json(message_bytes):
    """Safely deserializes Kafka bytes → Python dict"""
    try:
        return json.loads(message_bytes.decode("utf-8"))
    except:
        return {
            "_raw": message_bytes.decode("utf-8", errors="ignore"),
            "_error": "Failed to deserialize JSON"
        }
