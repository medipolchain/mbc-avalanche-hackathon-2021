from brownie import Game, network, config, accounts, exceptions
from scripts.helpful_scripts import get_account, deploy_mocks, LOCAL_BLOCKCHAIN_ENVIRONMENTS
import time
from web3 import Web3
import pytest


def deploy_fund_me():
    account = accounts.add(config["wallets"]["from_key"])  # accounts[0]  #   # accounts[0]  # get_account()
    # price_feed_address = config['networks']['rinkeby']['eth_usd_price_feed']

    a = time.time()
    game = Game.deploy("0x319fa2b82a1BA48bf17c5322C70FDeA231a012ee",
                       {"from": account},
                       publish_source=True)  # config['networks']['fuji']['verify']) #config["networks"][network.show_active()]["verify"])
    print(f"Deploying time: {time.time() - a}")


    return game


def main():
    print(deploy_fund_me())  # Contract Number

# .9723 > .9680
# .9433 > .9408
# .9356 > .
