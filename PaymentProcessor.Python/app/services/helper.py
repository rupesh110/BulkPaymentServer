MAX_RETRIES = 2

def _retry_or_dlq(producer, event, retry_topic, dead_letter, meta, key, reason):
    if not key:
        raise ValueError("Kafka key is required for retry/DLQ")

    meta["RetryCount"] += 1
    meta["LastFailureReason"] = reason

    retry_count = meta["RetryCount"]

    if retry_count > MAX_RETRIES:
        print(f"Sending to DLQ after {retry_count} attempts")
        producer.send(dead_letter, value=event, key=key)
        return {"status": "DEAD", "retryCount": retry_count}

    print(f"Retrying payment (attempt {retry_count})")
    producer.send(retry_topic, value=event, key=key)
    return {"status": "RETRYING", "retryCount": retry_count}
