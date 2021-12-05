from scripts.helpful_scripts import get_account, LOCAL_BLOCKCHAIN_ENVIRONMENTS
from scripts.deploy import deploy_fund_me
from brownie import network, accounts, exceptions
import pytest
from web3 import Web3
import brownie


def test_can_fund_and_withdraw2():
    account = accounts[0]
    game = deploy_fund_me()

    # with pytest.raises(exceptions.VirtualMachineError):
    tx = game.mintItem(
        [[5, 0, 0, 0], [0, 0, 0, 0], [5, 0, 0, 0], [0, 0, 0, 0], [0, 0, 0, 0], [0, 0, 0, 0], [0, 0, 0, 0], [0, 0, 0, 0]]
        , {"from": account})

    # tx = game.plant(0, 0, {"from": account})
    # tx1 = game.plant(0, 1, {"from": account})
    # tx2 = game.plant(0, 2, {"from": account})
    # tx3 = game.plant(0, 3, {"from": account})
    # tx4 = game.plant(1, 0, {"from": account})
    # tx5 = game.plant(1, 1, {"from": account})
    # tx6 = game.plant(1, 2, {"from": account})
    # tx7 = game.plant(1, 3, {"from": account})
    # tx8 = game.plant(2, 0, {"from": account})
    # tx9 = game.plant(2, 1, {"from": account})
    # tx10 = game.plant(2, 2, {"from": account})
    # tx11 = game.plant(2, 3, {"from": account})
    # tx12 = game.plant(3, 0, {"from": account})
    # tx13 = game.plant(3, 1, {"from": account})
    # tx14 = game.plant(3, 2, {"from": account})
    # tx15 = game.plant(3, 3, {"from": account})

    assert game.getSeedCollection({"from": account}) == ()

# def test_can_fund_and_withdraw():
#     # with pytest.raises(exceptions.VirtualMachineError):
#     account = accounts[0]
#     game = deploy_fund_me()
#     tx = game.calculatePrice(
#         [[1, 0, 0, 0], [0, 0, 0, 0], [0, 0, 0, 0], [0, 0, 0, 0], [0, 0, 0, 0], [0, 0, 0, 0], [0, 0, 0, 0],
#          [0, 0, 0, 0]], {"from": account})
#
#     assert tx == 100000000000000000

# def test_can_fund_and_withdraw():
#     # with pytest.raises(exceptions.VirtualMachineError):
#     account = accounts[0]
#     game = deploy_fund_me()
#     # entrance_fee = fund_me.getEntranceFee()  + 100
#     tx = game._auctionContract({"from": account})
#
#     assert tx == "0xd91d52e5A7d764225fa3bCee4Dcb685e6684C18c"
#
#
# def test_can_fund_and_withdraw2():
#     # with pytest.raises(exceptions.VirtualMachineError):
#     account = accounts[0]
#     game = deploy_fund_me()
#     # entrance_fee = fund_me.getEntranceFee()  + 100
#     tx = game._marketplaceContract({"from": account})
#
#     assert tx == "0xB1541E59Eb6912a3A080e3473567a631dbe3aFbd"
#
#
# def test_can_fund_and_withdraw3():
#     # with pytest.raises(exceptions.VirtualMachineError):
#     account = accounts[0]
#     game = deploy_fund_me()
#     # entrance_fee = fund_me.getEntranceFee()  + 100
#     tx = game._admin({"from": account})
#
#     assert tx == "0xd91d52e5A7d764225fa3bCee4Dcb685e6684C18c"

# def test_can_fund_and_withdraw5():
#     # with pytest.raises(exceptions.VirtualMachineError):
#     account = accounts[0]
#     game = deploy_fund_me()
#     # entrance_fee = fund_me.getEntranceFee()  + 100
#     with brownie.reverts("Farmville:  Only admin!"):
#         tx = game.setPrice(1,2,3, {"from": account})
#         print(tx.revert_msg)
#         assert tx.revert_msg == "Farmville:  Only admin!"

#
#     tx2 = fund_me.withdraw({"from": account})
#     tx2.wait(1)
#     assert fund_me.addressToAmountFunded(account.address) == 0
#
# def owner_test():
#     if network.show_active() not in LOCAL_BLOCKCHAIN_ENVIRONMENTS:
#         pytest.skip(f"Test {__name__} has been skipped as it requires the owner privileges.")
#
#     #account = get_account()
#     fund_me = deploy_fund_me()
#     bad_actor = accounts.add()
#
#     with pytest.raises(exceptions.VirtualMachineError):
#         fund_me.withdraw({"from": bad_actor})
