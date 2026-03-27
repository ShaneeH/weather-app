/**
 * Formats a date string into a human-readable local time (HH:MM).
 * @param dateString - A valid date string e.g. "2025-01-15 14:30"
 * @returns Formatted time string e.g. "14:30" or "2:30 PM" depending on locale
 */
export function formatTime(dateString: string): string {
  const date = new Date(dateString)

  return date.toLocaleTimeString([], {
    hour: "2-digit",
    minute: "2-digit",
  })
}