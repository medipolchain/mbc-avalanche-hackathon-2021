import json
from fastapi import FastAPI, HTTPException

app = FastAPI()


@app.get("/token/{item}")
def get_male(item: int):
    try:

        if item == 0:
            return {
                "name": "Wheat Seed",
                "description": "The seed of Wheat, which is used within the game MetaFarm to plant wheat.",
                "image": "https://gateway.ipfs.io/ipfs/QmePx13zz8QqU2QTPLjxtFKMVMZ47GaoAqsrPamReFstex",
            }

        elif item == 1:
            return {
                "name": "Corn Seed",
                "description": "The seed of Corn, which is used within the game MetaFarm to plant corn.",
                "image": "https://gateway.ipfs.io/ipfs/QmePx13zz8QqU2QTPLjxtFKMVMZ47GaoAqsrPamReFstex",
            }

        elif item == 2:
            return {
                "name": "Wheat",
                "description": "The Wheat, which is the version of the seed wheat after it is planted and grown in the game MetaFarm.",
                "image": "https://gateway.ipfs.io/ipfs/QmePx13zz8QqU2QTPLjxtFKMVMZ47GaoAqsrPamReFstex",
            }

        elif item == 3:
            return {
                "name": "Corn",
                "description": "The Corn, which is the version of the seed corn after it is planted and grown in the game MetaFarm.",
                "image": "https://gateway.ipfs.io/ipfs/QmePx13zz8QqU2QTPLjxtFKMVMZ47GaoAqsrPamReFstex",
            }

        elif item == 4:
            return {
                "name": "Cow",
                "description": "The cow animal to be used in the game MetaFarm.",
                "image": "https://gateway.ipfs.io/ipfs/QmbcPyiJUtyHWxFQV33JGCaxsXbxu9vYFz3voVojEHbbpH",
            }

        elif item == 5:
            return {
                "name": "Chicken",
                "description": "The chicken animal to be used in the game MetaFarm.",
                "image": "https://gateway.ipfs.io/ipfs/QmYhFX4EKwJJHcCv2snqnDFca6e6EiWUTmteb8wFhAYiHG",
            }

        elif item == 6:
            return {
                "name": "Windmill",
                "description": "The windmill to produce new products from the acquired plants to be used in the game MetaFarm.",
                "image": "",
            }

        elif item == 7:
            return {
                "name": "Feedmixer",
                "description": "The feedmixer to produce new products from the acquired plants to be used in the game MetaFarm.",
                "image": "https://gateway.ipfs.io/ipfs/QmYWV9mz5Tr8cV74NnKC4x5t526cEipEFN1RFtAqR9DiL7",
            }

        elif item == 8:
            return {
                "name": "Furnace",
                "description": "The furnace to produce new products from the acquired goods to be used in the game MetaFarm.",
                "image": "https://gateway.ipfs.io/ipfs/QmbD1pf5pZx6AcSg5W55rQ2tiArVud69BntgXiYyW15rKD",
            }

        elif item == 9:
            return {
                "name": "Bread",
                "description": "The bread to be sold or used to acquire new goods to be used in the game MetaFarm.",
                "image": "https://gateway.ipfs.io/ipfs/QmWWEVrdqE8UsU9TbjuEUYiyVLHi72CyU7ZkaJnTUo43dS",
            }

        elif item == 10:
            return {
                "name": "Popcorn",
                "description": "The popcorn to be sold or used in the game MetaFarm.",
                "image": "",
            }

        elif item == 11:
            return {
                "name": "Milk",
                "description": "The milk to be sold or used to acquire new goods to be used in the game MetaFarm.",
                "image": "https://gateway.ipfs.io/ipfs/QmQAPYrhqYm8SH9E9YuShpK9K1esGr9wxt1vVz8idm2oLu",
            }

        elif item == 12:
            return {
                "name": "Egg",
                "description": "The egg to be sold or used to acquire new goods to be used in the game MetaFarm.",
                "image": "https://gateway.ipfs.io/ipfs/QmSG7evoYeWgrzudvXADsh5UubqJAdFi5ZQjHXUifiEg85",
            }

        elif item == 13:
            return {
                "name": "Provender",
                "description": "The provender to be sold or feed the animals in the game MetaFarm to produce new goods.",
                "image": "",
            }

        elif item == 14:
            return {
                "name": "Dirt",
                "description": "The dirt, which is required to be bought to be able to plants the seeds and grow them.",
                "image": "",
            }

        elif item == 15:
            return {
                "name": "Cake",
                "description": "The cake, which can craft in game and sell it on marketplace.",
                "image": "https://gateway.ipfs.io/ipfs/QmesQiPiGDBf6YzbpKnukc7KpP6UEWYPqretGK7LjXnfUc",
            }

        elif item == 100:
            return {
                "name": "First Avatar",
                "description": "The dirt, which is required to be bought to be able to plants the seeds and grow them.",
                "image": "https://gateway.ipfs.io/ipfs/QmQPDHxjTWibdUbtyvJ23GRWGXaoNyb35hyeSrGo3oAKTf",
            }

        elif item == 101:
            return {
                "name": "Second Avatar",
                "description": "This is the avatar to be used in MetaFarm.",
                "image": "https://gateway.ipfs.io/ipfs/QmT1ndTSSMLj8ghKs5g8v3TW6evd4dXYHr8MbqRQ2SbyfD",
            }

        elif item == 102:
            return {
                "name": "Third Avatar",
                "description": "This is the avatar to be used in MetaFarm.",
                "image": "https://gateway.ipfs.io/ipfs/QmYTuJLxUwXP3A8tAsdsTY3CZo3fMErfo22PgzcNT7dSNa",
            }

        elif item == 103:
            return {
                "name": "Fourth Avatar",
                "description": "This is the avatar to be used in MetaFarm.",
                "image": "https://gateway.ipfs.io/ipfs/QmTkbsjD4eBnF8w3APWh6DuRzQzAd9KP4pZTGHvmDyPi97",
            }

        elif item == 104:
            return {
                "name": "Fifth Avatar",
                "description": "This is the avatar to be used in MetaFarm.",
                "image": "https://gateway.ipfs.io/ipfs/Qmdmqxnnxg6LVu3o28P5u7RxWQYmic1icrD1jsGsTaRfqf",
            }

    except Exception as e:
        return e
