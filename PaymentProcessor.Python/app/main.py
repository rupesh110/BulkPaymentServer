import os
from retry_paymentProcessor import run_retry_worker
from main_paymentprocessor import run_worker

def main():
	worker_type = os.getenv("WORKER_MODE", "main").lower()

	print(f"Starting worker in '{worker_type}' mode...")

	if worker_type == "retry":
		run_retry_worker()
	else:
		run_worker()

if __name__ == "__main__":
	main()