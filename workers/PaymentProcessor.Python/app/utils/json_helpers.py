import json

def safe_json(value, to_bytes=False):
    """
    - Consumer: bytes -> dict
    - Producer: dict -> bytes
    """
    if value is None:
        return None

    # Kafka consumer path (bytes -> dict)
    if isinstance(value, (bytes, bytearray)):
        try:
            return json.loads(value.decode("utf-8"))
        except Exception:
            return value.decode("utf-8", errors="ignore")

    # Kafka producer path (dict -> bytes)
    if to_bytes:
        return json.dumps(value).encode("utf-8")

    return value
