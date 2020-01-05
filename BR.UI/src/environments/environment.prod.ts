const baseUrl = 'https://8a310emepc.execute-api.eu-central-1.amazonaws.com/qa/api/v1/';

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
