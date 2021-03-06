// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

const baseUrl = 'http://localhost:5000/api/v1/';

export const environment = {
  production: false,
  loginUrl: baseUrl + 'iam/auth/login/', 
  authUrl: baseUrl + 'iam/auth/', 
  usersUrl: baseUrl + 'iam/users/', 
  beachesUrl: baseUrl + 'beaches/',
  reviewsUrl: baseUrl + 'reviews/',
  countriesUrl: baseUrl + 'places/countries/',
  placeChildrenUrl: baseUrl + 'places/{id}/next',
  searchUrl: baseUrl + 'search?query=',
  searchPlaceUrl: baseUrl + 'search/places?id={id}&name={name}&type={type}'
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
