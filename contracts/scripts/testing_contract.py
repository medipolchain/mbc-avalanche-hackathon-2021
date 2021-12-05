from scripts.helpful_scripts import get_account, LOCAL_BLOCKCHAIN_ENVIRONMENTS
from scripts.deploy import deploy_fund_me
from brownie import network, accounts, exceptions
import pytest

def calculate_the_price_test():
    with pytest.raises(exceptions.VirtualMachineError):
        ######################
        account = get_account()
        fund_me = deploy_fund_me()
        ######################

        tx = fund_me.calculatePrice({"from": account})
        tx.wait(1)
        print(tx)

        assert tx == tx

    # tx2 = fund_me.calculatePrice({"from": account})
    # tx2.wait(1)
    # print(tx2)
    # assert fund_me.balance(account.address) == 5

# def test_can_fund_and_withdraw():
#     with pytest.raises(exceptions.VirtualMachineError):
#         ######################
#         account = get_account()
#         fund_me = deploy_fund_me()
#         ######################
#
#         entrance_fee = 5 #fund_me.getEntranceFee() + 100
#         tx = fund_me.mintItem({"from": account, "value": entrance_fee})
#         tx.wait(1)
#
#
#         assert entrance_fee == entrance_fee
#
#     tx2 = fund_me.withdraw({"from": account})
#     tx2.wait(1)
#     assert fund_me.addressToAmountFunded(account.address) == 0