import logging

from requests import post
from email.message import EmailMessage
from email.utils import formatdate

from alerter.common import Alerter, AlerterFactory


@AlerterFactory.register
class Sms(Alerter):
    def __init__(self, **kwargs):
        super().__init__(**kwargs)
        self.recipient = kwargs.get('recipient')
        self.serverAddress = kwargs.get('serverAddress')

    @classmethod
    def from_args(cls, args):
        sender = args.email[0]
        recipients = args.email
        return cls(recipients=recipients)

    @classmethod
    def from_config(cls, config):
        recipient = config['recipient']
        serverAddress = config['serverAddress']
        return cls(recipient=recipient, serverAddress=serverAddress)

    @staticmethod
    def get_alerter_type():
        return 'sms'

    def __call__(self, **kwargs):
        set_subject = kwargs.get("subject")
        set_content = kwargs.get("content")

        requestContent = {
            "toPhoneNumber": self.recipient,
            "content": 'Subject: ' + set_subject + '\nMsg: ' + set_content
        }

        logging.debug(requestContent)
        logging.info("Sending request to Twilio at " + self.serverAddress, extra={ "body": requestContent })

        response = post(self.serverAddress, json=requestContent)

        logging.debug(response)

        if response.status_code != 200:
            raise Exception("Unexpected response: " + response.content.decode("utf-8"))
        else:
            logging.info("Response content: " + response.content.decode("utf-8"))