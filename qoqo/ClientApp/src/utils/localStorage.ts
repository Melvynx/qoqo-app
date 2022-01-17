export const getLocalStorage = <T>(key: string, defaultValue: T): T => {
  const value = localStorage.getItem(key);
  if (value === null) {
    return defaultValue;
  }

  try {
    const json = JSON.parse(value);
    if (typeof json !== typeof defaultValue) {
      return defaultValue;
    }
    return json as T;
  } catch (e) {
    return defaultValue;
  }
};

export const setLocalStorage = (key: string, value: unknown) => {
  localStorage.setItem(key, JSON.stringify(value));
};
