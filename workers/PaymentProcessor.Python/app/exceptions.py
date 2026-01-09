
class BusinessHandled(Exception):
    """
    Raised when a message was handled correctly
    (retry / DLQ / validation failure)
    and the worker should continue processing.
    """
    pass
