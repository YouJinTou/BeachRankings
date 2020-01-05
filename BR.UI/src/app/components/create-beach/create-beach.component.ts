import { Component, OnInit } from '@angular/core';
import { PlacesService } from '../../services/places.service';
import { Place } from '../../models/place';
import { CreateBeachModel } from '../../models/create.beach.model';
import { BeachesService } from '../../services/beaches.service';
import { UsersService } from '../../services/users.service';

@Component({
  selector: 'app-create-beach',
  templateUrl: './create-beach.component.html',
  styleUrls: ['./create-beach.component.css']
})
export class CreateBeachComponent implements OnInit {
  model: CreateBeachModel;
  countries: Place[];
  l1s: Place[];
  l2s: Place[];
  l3s: Place[];
  l4s: Place[];

  constructor(
    private placesService: PlacesService,
    private beachesService: BeachesService,
    private usersService: UsersService) { }

  ngOnInit() {
    this.model = new CreateBeachModel();

    this.usersService.currentUser.subscribe(u => {
      this.model.addedBy = u.id;
    });

    this.placesService.getCountries().subscribe(countries => {
      this.countries = countries;
    });
  }

  onCountryChanged(event: object) {
    let data = this.getOptionData(event);

    if (data['isDefault']) {
      this.clearPlaces(false);

      this.clearModelPlaceData();

      return;
    }

    this.model.country = data['text'];

    this.placesService.getNextLevel(data['value']).subscribe(ls => {
      this.l1s = ls;

      this.clearPlaces(false, false);
    });
  }

  onL1Changed(event: object) {
    let data = this.getOptionData(event);

    if (data['isDefault']) {
      this.clearPlaces(false, false);

      this.clearModelPlaceData(false);

      return;
    }

    this.model.l1 = data['value'];

    this.placesService.getNextLevel(data['value']).subscribe(ls => {
      this.l2s = ls;

      this.clearPlaces(false, false, false);
    });
  }

  onL2Changed(event: object) {
    let data = this.getOptionData(event);

    if (data['isDefault']) {
      this.clearPlaces(false, false, false);

      this.clearModelPlaceData(false, false);

      return;
    }

    this.model.l2 = data['value'];

    this.placesService.getNextLevel(data['value']).subscribe(ls => {
      this.l3s = ls;

      this.clearPlaces(false, false, false, false);
    });
  }

  onL3Changed(event: object) {
    let data = this.getOptionData(event);

    if (data['isDefault']) {
      this.clearPlaces(false, false, false, false);

      this.clearModelPlaceData(false, false, false);

      return;
    }

    this.model.l3 = data['value'];

    this.placesService.getNextLevel(data['value']).subscribe(ls => {
      this.l4s = ls;

      this.clearPlaces(false, false, false, false, false);
    });
  }

  onL4Changed(event: object) {
    let data = this.getOptionData(event);

    if (data['isDefault']) {
      this.clearModelPlaceData(false, false, false, false);
    } else {
      this.model.l4 = data['value'];
    }
  }

  addBeach() {
    this.beachesService.addBeach(this.model);
  }

  private clearModelPlaceData(
    clearCountry: boolean = true,
    clearL1: boolean = true,
    clearL2: boolean = true,
    clearL3: boolean = true,
    clearL4: boolean = true) {
    this.model.country = clearCountry ? null : this.model.country;
    this.model.l1 = clearL1 ? null : this.model.l1;
    this.model.l2 = clearL2 ? null : this.model.l2;
    this.model.l3 = clearL3 ? null : this.model.l3;
    this.model.l4 = clearL4 ? null : this.model.l4;
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

  private getOptionData(event: object) {
    let options = event['target'].options;
    let text = options[options.selectedIndex].text;
    let value = options[options.selectedIndex].value;

    return {
      'text': text,
      'value': value,
      'isDefault': value === '0'
    }
  }
}
