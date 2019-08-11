import { Continent } from './continent';
import { Country } from './country';
import { L1 } from './l1';
import { L2 } from './l2';
import { L3 } from './l3';
import { L4 } from './l4';
import { WaterBody } from './water.body';
import { Beach } from './beach';

export class SearchResult {
    continents: Continent[];
    countries: Country[];
    l1s: L1[];
    l2s: L2[];
    l3s: L3[];
    l4s: L4[];
    waterBodies: WaterBody[];
    beaches: Beach[];
}