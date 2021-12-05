// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;

import "@openzeppelin/contracts/token/ERC1155/ERC1155.sol";
import "@openzeppelin/contracts/utils/Counters.sol";
import "./lib/GameItemsData.sol";

contract Game is ERC1155 {
    using Counters for Counters.Counter;

    uint256[14] prices = [
        1 ether,
        1 ether,
        1 ether,
        1 ether,
        1 ether,
        1 ether,
        1 ether,
        1 ether,
        1 ether,
        1 ether,
        1 ether,
        1 ether,
        1 ether,
        1 ether
    ];
    uint256[] private times = [50, 50, 40, 70];

    Counters.Counter private _dirtIDs;
    // uint256 avatarIDCount

    // Mapping user dirts
    mapping(address => GameStructs.Dirt[]) public addressToDirt;
    mapping(address => GameStructs.Dirt[]) public addressToBusyDirt;
    mapping(address => uint256) public addressToAvatar;

    address private _marketplaceContract;
    address private _admin;

    event Test(uint256 index1);

    constructor(address marketplaceContract) ERC1155("backend-api-uri") {
        _admin = _msgSender();
        _marketplaceContract = marketplaceContract;
        _mint(_admin, GameStructs.avatar1, 1, "");
        _mint(_admin, GameStructs.avatar2, 1, "");
        _mint(_admin, GameStructs.avatar3, 1, "");
        _mint(_admin, GameStructs.avatar4, 1, "");
        _mint(_admin, GameStructs.avatar5, 1, "");
    }

    modifier onlyAdmin() {
        require(_msgSender() == _admin, "Farmville:  Only admin!");
        _;
    }

    function createAvatar(uint256 avatarID) public {
        require(balanceOf(_admin, avatarID) != 0, "Error");

        safeTransferFrom(_admin, _msgSender(), avatarID, 1, "");
    }

    function setPrice(uint256 amount, uint256 i) public onlyAdmin {
        require(amount != 0, "Farmbville: Price can not be 0.");
        require(i >= 0 && i < prices.length, "MetaFarm: Out of index.");
        prices[i] = amount;
    }

    function harvest() public {
        GameStructs.Dirt[] storage userBusyDirt = addressToBusyDirt[
            _msgSender()
        ];

        GameStructs.Dirt[] storage userAvailableDirt = addressToDirt[
            _msgSender()
        ];

        GameStructs.Dirt storage tempDirt;

        uint256 plant0Counter = 0;
        uint256 plant1Counter = 0;
        for (uint256 index = userBusyDirt.length - 1; index >= 0; index--) {
            tempDirt = userBusyDirt[index];

            if (isReachedExpireDate(tempDirt.expireDate)) {
                if (tempDirt.plantTypeID == 0) {
                    plant0Counter++;
                } else {
                    plant1Counter++;
                }
                userBusyDirt.pop();
                userAvailableDirt.push(
                    GameStructs.Dirt({expireDate: 0, plantTypeID: -1})
                );
            }
        }

        if (plant0Counter <= 0 && plant1Counter <= 0) {
            revert("MetaFarm: Nothing to harvest");
        }
        if (plant0Counter > 0) {
            _mint(_msgSender(), 0, plant0Counter, "");
            _mint(_msgSender(), 2, plant0Counter, "");
        }
        if (plant1Counter > 0) {
            _mint(_msgSender(), 1, plant1Counter, "");
            _mint(_msgSender(), 3, plant1Counter, "");
        }
    }

    function isReachedExpireDate(uint256 expireDate)
        public
        view
        returns (bool)
    {
        require(
            expireDate <= block.timestamp,
            "MetaFarm : Not harvest time"
        );
        return true;
    }

    function getExpireDate(uint256 plantTypeIndex)
        public
        view
        returns (uint256)
    {
        uint256 expireDate = block.timestamp + times[plantTypeIndex];

        return expireDate;
    }

    function massPlant(uint256 plantID0, uint256 plantID1) public {
        if (plantID0 <= 0 && plantID1 <= 0)
            revert("MetaFarm: Amounts can not be 0");
        if (plantID0 > 0) plant(0, plantID0);
        if (plantID1 > 0) plant(1, plantID1);
    }

    function plant(uint256 plantID, uint256 amount) public {
        GameStructs.Dirt[] storage userDirts = addressToDirt[msg.sender];
        GameStructs.Dirt[] storage userBusyDirts = addressToBusyDirt[
            msg.sender
        ];

        uint256 amountCount = amount;

        require(
            userDirts.length >= amount &&
                balanceOf(_msgSender(), plantID) >= amount,
            "Bad request"
        );

        GameStructs.Dirt storage tempDirt;
        for (uint256 index; index < userDirts.length; index++) {
            tempDirt = userDirts[index];
            if (amountCount > 0) {
                userDirts.pop();
                userBusyDirts.push(
                    GameStructs.Dirt({
                        expireDate: getExpireDate(plantID),
                        plantTypeID: int256(plantID)
                    })
                );
                amountCount -= 1;
            } else {
                break;
            }
        }

        _burn(_msgSender(), plantID, amount);
    }

    function mintItem(uint256[14] memory amounts) public payable {
        uint256 totalPrice = calculatePrice(amounts);
        require(totalPrice == msg.value, "FarmVille: Must send correct price.");
        for (uint256 i; i < amounts.length - 1; i++) {
            uint256 amount = amounts[i];
            if (amount > 0) _mint(_msgSender(), i, amount, "");
        }
        if (amounts[amounts.length - 1] != 0) {
            addressToDirt[_msgSender()].push(
                GameStructs.Dirt({expireDate: 0, plantTypeID: -1})
            );
        }
    }

    function craftBread(uint256 wheatAmount) public {
        require(
            balanceOf(msg.sender, 6) != 0 &&
                balanceOf(msg.sender, 2) > wheatAmount &&
                wheatAmount % 2 == 0,
            "MetaFarm: You need to have windmill for craft bread"
        );

        _burn(_msgSender(), 2, wheatAmount);
        _mint(msg.sender, 9, wheatAmount / 2, "");
    }

    function craftPopcorn(uint256 cornAmount) public {
        require(
            balanceOf(msg.sender, 8) != 0 &&
                balanceOf(msg.sender, 3) > cornAmount &&
                cornAmount % 3 == 0,
            "MetaFarm: You need to have windmill for craft bread"
        );

        _burn(_msgSender(), 3, cornAmount);
        _mint(msg.sender, 10, cornAmount / 3, "");
    }

    function craftFeed(uint256 wheatAmount) public {
        require(
            balanceOf(msg.sender, 7) != 0 &&
                balanceOf(msg.sender, 2) > wheatAmount &&
                wheatAmount % 2 == 0,
            "MetaFarm: You need to have windmill for craft bread"
        );

        _burn(_msgSender(), 2, wheatAmount);
        _mint(msg.sender, 13, wheatAmount / 2, "");
    }

    function feedChicken() public {
        require(
            balanceOf(_msgSender(), 5) != 0 && balanceOf(msg.sender, 13) >= 2,
            "Error!!"
        );

        _burn(msg.sender, 13, 2);
        _mint(msg.sender, 12, 1, "");
    }

    function feedCow() public {
        require(
            balanceOf(_msgSender(), 4) != 0 && balanceOf(msg.sender, 13) >= 3,
            "Error!!"
        );

        _burn(msg.sender, 13, 3);
        _mint(msg.sender, 11, 1, "");
    }

    function calculatePrice(uint256[14] memory items)
        public
        view
        returns (uint256)
    {
        uint256 balance = 0;

        for (uint256 i; i < items.length; i++) {
            balance += prices[i] * items[i];
        }

        return balance;
    }

    function getDirtCollection()
        public
        view
        returns (GameStructs.Dirt[] memory)
    {
        return addressToDirt[_msgSender()];
    }
}