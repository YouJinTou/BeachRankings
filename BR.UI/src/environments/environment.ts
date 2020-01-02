// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  loginUrl: 'http://localhost:5000/api/v1/iam/auth/login/', 
  authUrl: 'http://localhost:5000/api/v1/iam/auth/', 
  usersUrl: 'http://localhost:5000/api/v1/iam/users/', 
  beachesUrl: 'http://localhost:5002/api/v1/beaches/',
  reviewsUrl: 'http://localhost:5004/api/v1/reviews/',
  countriesUrl: 'http://localhost:5006/api/v1/places/countries/',
  placeChildrenUrl: 'http://localhost:5006/api/v1/places/{id}/next',
  searchUrl: 'http://localhost:5008/api/v1/search?query=',
  searchPlaceUrl: 'http://localhost:5008/api/v1/search/place?id={id}&name={name}',
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
