				Client
				   |
				   V
				Bulkpayment api
					|
					V

		Kafka | Blog storage | Azure sql
				    |
					V
				payment processor (Worker)
				    |
					V
			---- ---------
			|			|
			Main		Retry
