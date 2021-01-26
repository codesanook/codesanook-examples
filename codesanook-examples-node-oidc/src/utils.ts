import base64Url from 'base64url'
import { createHash } from 'crypto';

/**
 * Return a unique identifier with the given `len`.
 *
 * utils.uid(10);
 * // => "FDaS435D2z"
 *
 * @param {Number} len
 * @return {String}
 * @api private
 */
export const uid = function (len) {
  var buf = []
    , chars = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789@#$%!'
    , charlen = chars.length;

  for (var i = 0; i < len; ++i) {
    buf.push(chars[getRandomNumber(0, charlen - 1)]);
  }

  return buf.join('');
};

/**
 * Return a random int, used by `utils.uid()`
 *
 * @param {Number} min
 * @param {Number} max
 * @return {Number}
 * @api private
 */

function getRandomNumber(min, max) {
  return Math.floor(Math.random() * (max - min + 1)) + min;
}

export function hashCodeVerify(codeVerifier: string, codeChallengeMethod: string) {
  const hash = createHash(codeChallengeMethod).update(codeVerifier).digest();
  return base64Url.encode(hash);
}
