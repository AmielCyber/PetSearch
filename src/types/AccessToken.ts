interface AccessToken {
  token: string;
  expirationDate: Date;
}

export interface StoredAccessToken {
  token: string;
  expirationDateString: string;
}
export default AccessToken;
