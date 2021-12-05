// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;

library GameStructs {
    uint256 public constant WHEATSEED = 0;
    uint256 public constant CORNSEED = 1;
    uint256 public constant WHEATPLANT = 2;
    uint256 public constant CORNPLANT = 3;
    uint256 public constant COW = 4;
    uint256 public constant CHICKEN = 5;
    uint256 public constant WINDMILL = 6;
    uint256 public constant FEEDMIXER = 7;
    uint256 public constant FURNANCE = 8;
    uint256 public constant BREAD = 9;
    uint256 public constant POPCORN = 10;
    uint256 public constant MILK = 11;
    uint256 public constant EGG = 12;
    uint256 public constant FEED = 13;
    uint256 public constant DIRT = 14;

    struct Dirt {
        uint256 expireDate;
        int256 plantTypeID;
    }

    uint256 public constant avatar1 = 100;
    uint256 public constant avatar2 = 101;
    uint256 public constant avatar3 = 102;
    uint256 public constant avatar4 = 103;
    uint256 public constant avatar5 = 104;
}