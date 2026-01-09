import logging

logger = logging.getLogger(__name__)

MAX_RETRIES = 2


def retry_or_dlq(producer, event, retry_topic, dead_letter, meta, key, reason):
    if not key:
        raise ValueError("Kafka key is required for retry/DLQ")

    meta["RetryCount"] += 1
    meta["LastFailureReason"] = reason

    retry_count = meta["RetryCount"]

    logger.debug(
        "Retry decision",
        extra={
            "retryCount": retry_count,
            "maxRetries": MAX_RETRIES,
            "key": key,
            "reason": reason,
        },
    )

    if retry_count > MAX_RETRIES:
        producer.send(dead_letter, value=event, key=key.encode("utf-8"))
        return {"status": "DEAD", "retryCount": retry_count}

    producer.send(retry_topic, value=event, key=key.encode("utf-8"))
    return {"status": "RETRYING", "retryCount": retry_count}
