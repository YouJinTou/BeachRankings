import { Injectable } from '@angular/core';
import { Place } from '../models/place';

@Injectable({
  providedIn: 'root'
})
export class PlacesService {

  constructor() { }

  public getCountries(): Place[] {
    return [
      new Place('1', 'Bulgaria'),
      new Place('2', 'Romania')
    ]
  }

  public getLevelOnes(countryId: string): Place[] {
    if (countryId == '1') {
      return [
        new Place('3', 'Varna'),
      ]
    }

    return [
      new Place('4', 'Constanta'),
    ]
  }

  public getLevelTwos(l1Id: string): Place[] {
    if (l1Id == '3') {
      return [
        new Place('5', 'Varna'),
      ]
    }
    
    return [
      new Place('6', 'Mamaia'),
    ]
  }

  public getLevelThrees(l2Id: string): Place[] {
    if (l2Id == '5') {
      return [
        new Place('7', 'St. St. Constantine and Helena'),
      ]
    }
    
    return []
  }

  public getLevelFours(l3Id: string): Place[] {
    return []
  }
}
