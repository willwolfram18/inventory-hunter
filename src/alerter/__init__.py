import alerter.discord
import alerter.emailer
import alerter.sendgridemailer
import alerter.slack
import alerter.sms
import alerter.telegram

from alerter.common import AlerterFactory


def init_alerters(args):
    return AlerterFactory.create(args)
