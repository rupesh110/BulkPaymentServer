				Client
				   |
				   V
				Bulkpayment api
					|
					V

		Kafka | Blog storage | Azure sql
				    |
					V
				payment processor
				    |
					V
			---- ---------
			|			|
			Main		Retry
