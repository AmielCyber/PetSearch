import type AccessToken from "../models/accessToken";
import type { StoredAccessToken } from "../models/accessToken";

const CACHE_NAME = "app-cache";

export function tokenIsExpired(accessToken: AccessToken): boolean {
  if (!accessToken?.expirationDate) {
    // No AccessToken
    localStorage.removeItem(CACHE_NAME);
    return true;
  }
  if (Date.now() >= accessToken.expirationDate.getTime()) {
    // Expired.
    localStorage.removeItem(CACHE_NAME);
    return true;
  }
  return false;
}

/**
 * Gets a browser local stored token.
 * Removes a stored token if it has expired.
 * @returns AccessToken from stored location if token is valid,
 * else returns null if stored token has expired or there is no stored token.
 */
export function getStoredToken(): AccessToken | null {
  let accessToken: AccessToken | null = null;
  const cache = localStorage.getItem(CACHE_NAME);

  if (cache !== null) {
    // If there is a cache stored.
    const accessTokenCache: StoredAccessToken = JSON.parse(cache);
    if (accessTokenCache && accessTokenCache.token && accessTokenCache.expirationDateString) {
      accessToken = {
        token: accessTokenCache.token,
        expirationDate: new Date(accessTokenCache.expirationDateString),
      };
      if (tokenIsExpired(accessToken)) {
        accessToken = null;
      }
    }
  }
  return accessToken;
}

export function storeAccessToken(accessToken: AccessToken) {
  localStorage.setItem(
    CACHE_NAME,
    JSON.stringify({
      token: accessToken.token,
      expirationDateString: accessToken.expirationDate.toISOString(),
    })
  );
}
