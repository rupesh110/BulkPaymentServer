import os
import logging
from dotenv import load_dotenv

from retry_paymentProcessor import run_retry_worker
from main_paymentprocessor import run_worker
from utils.logging import setup_logging


def main():
    # Always safe
    load_dotenv()

    setup_logging()
    logger = logging.getLogger(__name__)

    worker_type = os.getenv("WORKER_MODE", "main").lower()
    logger.info("Starting worker", extra={"mode": worker_type})

    if worker_type == "retry":
        run_retry_worker()
    else:
        run_worker()


if __name__ == "__main__":
    main()
