import { getBaseUrl } from '../main';

type config = {
  data?: unknown;
  token?: string | null;
  method?: 'GET' | 'POST' | 'PUT' | 'DELETE' | 'OPTIONS' | 'PATCH';
  headers?: HeadersInit;
  customConfig?: RequestInit;
  json?: boolean;
};

/*
  To call the api.

  To `get` an item
  ```ts
  client("item")
  ```
  Get is the default method if no data provided.

  If you want `POST` an item, you need to provide data.
  If Data was provided, default method is `POST`
  ```ts
  client("item", {data: {name: "John"}})
  ```

  To `patch` / `delete` an item
  ```ts
  client("item/1", {method: "PATCH"})
  ```

  To provide a Token :
  ```ts
  client("item/1", {method: "PATCH", token: "SomeToken"})
  ```

  TODO: You can see example in `client.test.tsx`
*/

async function client<T>(
  endpoint: string,
  {
    data,
    method,
    token,
    headers: customHeaders,
    customConfig,
    json = true,
  }: config = {}
): Promise<T> {
  const config: RequestInit = {
    method: method || (data ? 'POST' : 'GET'),
    body: data ? JSON.stringify(data) : null,
    headers: {
      Authorization: token ? `Bearer ${token}` : '',
      'Content-Type': data ? 'application/json' : '',
      Accept: 'application/json',
      ...customHeaders,
    },
    ...customConfig,
  };

  return window
    .fetch(`${getBaseUrl()}api/${endpoint}`, config)
    .then(async (response) => {
      // statusCode "401" is for unauthenticated request
      if (response.status === 401) {
        return Promise.reject({ message: 'Please re-authenticate.' });
      }

      const data = json ? await response.json() : await response.text();

      if (response.ok) {
        return Promise.resolve(data);
      } else {
        return Promise.reject(data);
      }
    });
}

export { client };
