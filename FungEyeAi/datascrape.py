import pandas as pd
import requests
import os
import random

headers = {
    'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/64.0.3282.140 Safari/537.36 Edge/18.17720'
}

data = pd.read_csv('data.csv')
df = pd.DataFrame(data)

# # sort values for tests
# df = df.sort_values('name')

urls = df['image'].tolist()
names = df['name'].tolist()

for url, name in zip(urls, names):
    response = requests.get(url, headers=headers)
    response.raise_for_status()
    if response.status_code == 200:
        if not os.path.exists(f'images/{name.replace(" ", "_")}'):
            os.makedirs(f'images/{name.replace(" ", "_")}')
        
        filename = f'images/{name.replace(" ", "_")}/{os.path.basename(url)}'
        with open(filename, 'wb') as f:
            f.write(response.content)
    else:
        print(f'Failed to download {name}, {url}') 