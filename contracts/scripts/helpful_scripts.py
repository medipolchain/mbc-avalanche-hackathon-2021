from brownie import network, config, accounts
from web3 import Web3

DECIMALS = 8
STARTING_PRICE = 200000000000

FORKED_LOCAL_ENVIRONMENTS = ['mainnet-fork', 'mainnet-fork-dev']
LOCAL_BLOCKCHAIN_ENVIRONMENTS = ['development', 'ganache-local']

def get_account():
    if network.show_active() == "development":
        return accounts[0]
    else:
        return accounts.add(config["wallets"]["from_key"])


def deploy_mocks():
    print(f"The active network is {network.show_active()}")
    print("Deploying the Mocks...")

    if len(MockV3Aggregator) <= 0:
        MockV3Aggregator.deploy(
            18, Web3.toWei(2000, 'ether'), {"from": get_account()}
        )
    print("Mocks deployed!")
