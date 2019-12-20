import { Component, OnInit } from '@angular/core';
import { PlacesService } from '../../services/places.service';
import { Place } from '../../models/place';

@Component({
  selector: 'app-create-beach',
  templateUrl: './create-beach.component.html',
  styleUrls: ['./create-beach.component.css']
})
export class CreateBeachComponent implements OnInit {
  countries: Place[]
  l1s: Place[]
  l2s: Place[]
  l3s: Place[]
  l4s: Place[]

  constructor(private placesService: PlacesService) { }

  ngOnInit() {
    this.countries = this.placesService.getCountries()
  }

  onCountryChanged(id: string) {
    if (id == '0') {
      this.clearPlaces(false);

      return;
    }

    this.l1s = this.placesService.getLevelOnes(id);

    this.clearPlaces(false, false);
  }

  onL1Changed(id: string) {
    if (id == '0') {
      this.clearPlaces(false, false);

      return;
    }

    this.l2s = this.placesService.getLevelTwos(id);

    this.clearPlaces(false, false, false);
  }

  onL2Changed(id: string) {
    if (id == '0') {
      this.clearPlaces(false, false, false);

      return;
    }

    this.l3s = this.placesService.getLevelThrees(id);

    this.clearPlaces(false, false, false, false);
  }

  onL3Changed(id: string) {
    if (id == '0') {
      this.clearPlaces(false, false, false, false);

      return;
    }

    this.l4s = this.placesService.getLevelFours(id);

    this.clearPlaces(false, false, false, false, false);
  }

  private clearPlaces(
    clearCountries: boolean = true, 
    clearL1s: boolean = true, 
    clearL2s: boolean = true, 
    clearL3s: boolean = true, 
    clearL4s: boolean = true) {
    this.countries = clearCountries ? null : this.countries;
    this.l1s = (clearL1s || !this.l1s.length) ? null : this.l1s;
    this.l2s = (clearL2s || !this.l2s.length) ? null : this.l2s;
    this.l3s = (clearL3s || !this.l3s.length) ? null : this.l3s;
    this.l4s = (clearL4s || !this.l4s.length) ? null : this.l4s;
  }
}
