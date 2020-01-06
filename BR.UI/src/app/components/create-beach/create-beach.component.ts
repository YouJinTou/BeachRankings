import { Component, OnInit } from '@angular/core';
import { PlacesService } from '../../services/places.service';
import { Place } from '../../models/place';
import { CreateBeachModel } from '../../models/create.beach.model';
import { BeachesService } from '../../services/beaches.service';
import { UsersService } from '../../services/users.service';
import { ActivatedRoute } from '@angular/router';
import { FormGroup, FormBuilder, FormControl } from '@angular/forms';

@Component({
  selector: 'app-create-beach',
  templateUrl: './create-beach.component.html',
  styleUrls: ['./create-beach.component.css']
})
export class CreateBeachComponent implements OnInit {
  placesForm: FormGroup;
  model: CreateBeachModel;
  countries: Place[];
  l1s: Place[];
  l2s: Place[];
  l3s: Place[];
  l4s: Place[];

  constructor(
    private placesService: PlacesService,
    private beachesService: BeachesService,
    private usersService: UsersService,
    private route: ActivatedRoute,
    private formBuilder: FormBuilder) {
    this.model = new CreateBeachModel();
    this.placesForm = formBuilder.group({
      coordinates: new FormControl(),
      countryList: 0,
      l1List: 0,
      l2List: 0,
      l3List: 0,
      l4List: 0
    });
  }

  ngOnInit() {
      this.usersService.currentUser.subscribe(u => {
        this.model.addedBy = u.id;
      });

      this.placesService.getCountries().subscribe(countries => {
        this.countries = countries;

        this.route.url.subscribe(v => {
          if (v.length == 3 && v[2].path == 'edit') {
            this.route.params.subscribe(p => {
              this.beachesService.getBeach(p['id']).subscribe(b => {
                this.model.name = b.name;
                this.model.l1 = b.l1;
                this.model.l2 = b.l2;
                this.model.l3 = b.l3;
                this.model.l4 = b.l4;
                this.model.country = b.country;
                this.model.coordinates = b.coordinates;
                let group = {};
                group['countryList'] = b.country ? b.country : 0;
                group['coordinates'] = b.coordinates;

                if (b.l1) {
                  this.placesService.getNextLevel(b.countryId).subscribe(l1s => {
                    this.l1s = l1s;
                    group['l1List'] = b.l1Id;
                    this.placesForm = this.formBuilder.group(group);
                  });
                }

                if (b.l2) {
                  this.placesService.getNextLevel(b.l1Id).subscribe(l2s => {
                    this.l2s = l2s;
                    group['l2List'] = b.l2Id;
                    this.placesForm = this.formBuilder.group(group);
                  });
                }

                if (b.l3) {
                  this.placesService.getNextLevel(b.l2Id).subscribe(l3s => {
                    this.l3s = l3s;
                    group['l3List'] = b.l3Id;
                    this.placesForm = this.formBuilder.group(group);
                  });
                }

                if (b.l4) {
                  this.placesService.getNextLevel(b.l3Id).subscribe(l4s => {
                    this.l4s = l4s;
                    group['l4List'] = b.l4Id;
                    this.placesForm = this.formBuilder.group(group);
                  });
                }
              });
            });
          }
      });
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

  submit() {
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
