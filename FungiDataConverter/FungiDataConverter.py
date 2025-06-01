import csv

input_file = 'mushroom_description.txt'
output_file = 'mushroom_description.csv'

with open(input_file, 'r', encoding='utf-8') as infile, open(output_file, 'w', newline='', encoding='utf-8') as outfile:
    writer = csv.writer(outfile, delimiter=';')
    writer.writerow(['LatinName', 'PolishName', 'Description', 'Edibility', 'Toxicity', 'Habitat'])

    for line in infile:
        if line.strip():
            parts = line.split(' - ')
            if len(parts) == 6:
                parts = [part.replace('"', '') for part in parts]
                writer.writerow(parts)
